<template>
    <v-dialog
            :value="value"
            @input="cancel"
            max-width="600px"
    >
        <v-card>
            <v-card-title>
                <div>
                    <h3 class="headline primary--text">Account</h3>
                </div>
            </v-card-title>
            <v-card-text>
                <v-form
                        ref="form"
                        v-if="account"
                        v-model="validFormData"
                >
                    <v-container grid-list-md>
                        <v-layout wrap>
                            <v-flex xs12>
                                <v-text-field
                                        v-model="account.name"
                                        label="Name"
                                        :rules="[v => !!v || 'Give the account a name!']"
                                        required
                                />
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field
                                        v-model.number="account.iban"
                                        label="IBAN"
                                        mask="AA## #### #### #### #### #"
                                        :rules="[v => !!v || 'Make sure to provide the correct iban!']"
                                        required
                                />
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field
                                        v-model.number="account.currentBalance"
                                        label="Current Balance"
                                        type="number"
                                        :rules="[v => !!v || 'Give the accounts current balance!']"
                                        required
                                />
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field
                                        v-model="account.note"
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
                <v-btn color="primary" flat :disabled="!validFormData" @click="submit">save</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import {Account} from "../../types/Account";

    @Component
    export default class AccountForm extends Vue {
        @Prop()
        account: Account;

        @Prop()
        value: boolean;

        validFormData: boolean = true;

        submit() {
            if ((<HTMLFormElement>this.$refs.form).validate()) {
                this.$emit('save', this.account)
            }
        }

        cancel() {
            (<HTMLFormElement>this.$refs.form).reset();
            this.$emit('input');
        }
    }
</script>