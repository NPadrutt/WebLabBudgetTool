<template>
    <v-container>
        <v-layout row wrap>
            <v-flex xs12>
                <payment-list
                        :payments="payments"
                />
            </v-flex>
            <v-flex xs12>
                <v-btn @click="newPayment">
                    Add Payment
                </v-btn>
                <payment-form
                        v-model="showForm"
                        :payment="paymentForForm"
                        :accounts="accounts"
                        :categories="categories"
                        @save="savePayment"
                />
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import ApiService from "../../service/ApiService";
    import {Payment, PaymentType, PaymentWithReferences} from "../../types/Payment";
    import {Category} from "../../types/Category";
    import PaymentList from "./PaymentList";
    import PaymentForm from "./PaymentForm";

    @Component({
        components: {
            PaymentList,
            PaymentForm
        }
    })
    export default class Payments extends Vue {
        payments: Payment[] = [];
        accounts: Account[] = [];
        accountsById: object = {};
        categories: Category[] = [];
        categoriesById: object = {};

        showForm: boolean = false;
        paymentForForm: PaymentWithReferences | null = null;



        mounted() {
            this.$emit('mounted', 'Payments');
            Promise.all([this.loadPayments(), this.loadAccounts(), this.loadCategories()])
                .then(values => Promise.all(values.map(value => value.json())))
                .then(values => {
                    this.accounts = values[1];
                    for (let account of values[1]) {
                        this.accountsById[account.id] = account;
                    }

                    this.categories = values[2];
                    for (let category of values[2]) {
                        this.categoriesById[category.id] = category;
                    }

                    this.saveAndMapPayments(values[0]);
                })
                .catch(error => {
                    console.error('Loading payments with associated accounts and categories failed with error: ' + error);
                });
        }

        loadPayments() {
            return ApiService.makeRequest('payments');
        }

        loadAccounts() {
            return ApiService.makeRequest('accounts');
        }

        loadCategories() {
            return ApiService.makeRequest('categories')
        }

        saveAndMapPayments(payments: Payment[]) {
            this.payments = payments;
            this.payments.map(payment => {
                payment.account = this.accountsById[payment.accountId];
                payment.category = payment.categoryId ? this.categoriesById[payment.categoryId] : null;
                return payment;
            })
        }

        newPayment() {
            let payment = <PaymentWithReferences>{
                id: 0,
                accountId: 0,
                date: (new Date()).toISOString(),
                amount: undefined,
                isCleared: false,
                type: PaymentType.EXPENSE,
                note: ''
            };
            this.editPayment(payment);
        }

        editPayment(payment: PaymentWithReferences) {
            this.paymentForForm = payment;
            this.showForm = true;
        }

        cancelEdit() {
            this.paymentForForm = null;
            this.showForm = false;
        }

        savePayment(payment: PaymentWithReferences, transferTargetAccountId: number) {
            let requests: Promise<Response>[] = [];
            if (payment.type === PaymentType.TRANSFER) {
                payment.type = PaymentType.TRANSFER_OUT;

                let paymentTransferIn = JSON.parse(JSON.stringify(payment));
                paymentTransferIn.accountId = transferTargetAccountId;
                paymentTransferIn.type = PaymentType.TRANSFER_IN;

                requests.push(this.persistPayment(paymentTransferIn));
            }

            requests.push(this.persistPayment(payment));
            Promise.all(requests)
                .then(values => {
                    return this.loadPayments();
                })
                .then(response => response.json())
                .then(payments => {
                    this.saveAndMapPayments(payments);
                    this.showForm = false;
                });
        }

        persistPayment(payment: PaymentWithReferences) {
            return ApiService.makeRequest('payments', payment, 'POST');
        }
    }
</script>