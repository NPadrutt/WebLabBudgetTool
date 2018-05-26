<template>
    <v-container>
        <v-layout row wrap>
            <v-flex xs12 sm6 pa-1 v-for="account of accounts" :key="account.id">
                <v-card>
                    <v-card-title primary-title>
                        <div>
                            <h3 class="headline mb-0">{{account.name}}</h3>
                            <div class="grey--text">{{account.iban}}</div>
                        </div>
                    </v-card-title>
                    <v-card-text>
                        <div>
                            <p>{{account.note}}</p>
                            <p class="title text-xs-right">{{formatCurrency(account.currentBalance)}}</p>
                        </div>
                    </v-card-text>
                </v-card>
            </v-flex>
        </v-layout>
        <v-layout row wrap>
            <v-flex>
                <v-btn @click="newAccount">
                    New Account
                </v-btn>
                <account-form
                        v-model="showForm"
                        :account="accountForForm"
                        @save="saveAccount"
                />
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import {Account} from "../../types/Account";
    import AccountForm from "./AccountForm"
    import ApiService from "../../service/ApiService";


    @Component({
        components: {
            AccountForm
        }
    })
    export default class Accounts extends Vue {
        accounts: Account[] = [];
        showForm: boolean = false;
        accountForForm: Account | null = null;

        mounted() {
            this.loadAccounts();
        }

        loadAccounts() {
            ApiService.makeRequest('accounts')
                .then(response => response.json())
                .then(json => {
                    this.accounts = json
                });
        }

        formatCurrency(value) {
            let val = (value / 1).toFixed(2);
            return 'CHF ' + val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "'")
        }

        newAccount() {
            let account = <Account>{
                id: 0,
                name: '',
                iban: '',
                currentBalance: undefined,
                note: '',
                isOverdrawn: false,
                isExcluded: false
            };
            this.editAccount(account);
        }

        editAccount(account: Account) {
            this.accountForForm = account;
            this.showForm = true;
        }

        cancelEdit() {
            this.accountForForm = null;
            this.showForm = false;
        }

        saveAccount(account: Account) {
            ApiService.makeRequest('accounts', account, 'POST')
                .then(response => {
                    this.loadAccounts();
                    this.showForm = false;
                });

        }
    }
</script>