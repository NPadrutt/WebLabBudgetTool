<style lang="css">
    .btn-toggle {
        width: 100%;
    }

    .btn-toggle .btn {
        width: 33.333333%;
    }
</style>

<template>
    <v-dialog
            :value="value"
            @input="cancel"
            max-width="600px"
    >
        <v-card>
            <v-card-title>
                <div>
                    <h3 class="headline">Payment</h3>
                </div>
            </v-card-title>
            <v-card-text>
                <v-form
                        ref="form"
                        v-if="payment"
                        v-model="validFormData"
                >
                    <v-container grid-list-md>
                        <v-layout wrap>
                            <v-flex xs12>
                                <v-btn-toggle class="mb-3" block v-model="payment.type">
                                    <v-btn flat :value="paymentTypes.INCOME">
                                        Incoming
                                    </v-btn>
                                    <v-btn flat :value="paymentTypes.EXPENSE">
                                        Outgoing
                                    </v-btn>
                                    <v-btn flat :value="paymentTypes.TRANSFER">
                                        Account Transfer
                                    </v-btn>
                                </v-btn-toggle>
                            </v-flex>
                            <v-flex xs12>
                                <v-select
                                        :items="accounts"
                                        v-model="payment.accountId"
                                        :label="getAccountLabel()"
                                        item-text="name"
                                        item-value="id"
                                        single-line
                                        :rules="[v => !!v || 'Please choose an account!']"
                                        required
                                ></v-select>
                            </v-flex>
                            <v-flex xs12>
                                <v-select
                                        v-if="payment.type === paymentTypes.TRANSFER"
                                        :items="accounts"
                                        v-model="transferTargetAccountId"
                                        label="Target account"
                                        item-text="name"
                                        item-value="id"
                                        single-line
                                        :rules="[v => !!v || 'Please choose an account!']"
                                        required
                                ></v-select>
                            </v-flex>
                            <v-flex xs12>
                                <v-select
                                        v-if="payment.type !== paymentTypes.TRANSFER"
                                        :items="categories"
                                        v-model="payment.categoryId"
                                        label="Category"
                                        item-text="name"
                                        item-value="id"
                                        single-line
                                        autocomplete
                                ></v-select>
                            </v-flex>
                            <v-flex xs12>
                                <date-picker
                                        :date="payment.date"
                                        label="Date"
                                        :rules="[v => !!v || 'Please choose a date!']"
                                        required
                                />
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field
                                        v-model.number="payment.amount"
                                        label="Amount"
                                        type="number"
                                        suffix="CHF"
                                        :rules="[v => !!v || 'Please enter a valid amount!']"
                                        required
                                />
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field
                                        v-model="payment.note"
                                        label="Notes"
                                />
                            </v-flex>
                        </v-layout>
                    </v-container>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer/>
                <v-btn flat @click="cancel">cancel</v-btn>
                <v-btn color="indigo" flat :disabled="!validFormData" @click="submit">save</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import {PaymentType, PaymentWithReferences} from "../../types/Payment";
    import DatePicker from "../DatePicker";
    import {Category} from "../../types/Category";
    import {Account} from "../../types/Account";

    @Component({
        components: {DatePicker}
    })
    export default class PaymentForm extends Vue {
        @Prop()
        payment: PaymentWithReferences;

        @Prop()
        accounts: Account[];

        @Prop()
        categories: Category[];

        @Prop()
        value: boolean = false;

        validFormData: boolean = true;

        paymentTypes = PaymentType;

        transferTargetAccountId?: Account = undefined;

        getAccountLabel() {
            return this.payment.type === PaymentType.INCOME ? 'Target Account' : 'Source Account';
        }

        submit() {
            if ((<HTMLFormElement>this.$refs.form).validate()) {
                this.$emit('save', this.payment, this.transferTargetAccountId);
            }
        }

        cancel() {
            (<HTMLFormElement>this.$refs.form).reset();
            this.$emit('input');
        }
    }

</script>