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
                        :showForm="showForm"
                        :account="accountForForm"
                        @cancel="cancelEdit"
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
            "account-form": AccountForm
        }
    })
    export default class Accounts extends Vue {
        accounts: Account[] = [<Account>{
            id: 0,
            name: 'Sparkonto',
            iban: 'CH22 8119 2000 0008 6592 4',
            currentBalance: 2038.55,
            note: "Persönliches Sparkonto",
            isOverdrawn: false,
            isExcluded: false
        }];

        showForm: boolean = false;
        accountForForm: Account | null = null;
        apiService: ApiService = new ApiService();

        formatCurrency(value) {
            let val = (value / 1).toFixed(2);
            return 'CHF ' + val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "'")
        }

        newAccount() {
            let account = <Account>{
                id: 0,
                name: '',
                iban: '',
                currentBalance: 0,
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
            // TODO: implement save
            let response = this.apiService.makeRequest('accounts', account, 'POST');
            console.log(response);
            this.showForm = false;
        }
    }
</script>