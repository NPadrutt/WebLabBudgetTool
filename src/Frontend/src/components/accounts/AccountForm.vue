<template>
    <v-dialog v-if="showForm" v-model="showForm">
        <v-card>
            <v-card-title>
                <div>
                    <h3 class="headline">Account</h3>
                </div>
            </v-card-title>
            <v-card-text>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-text-field
                                v-model="account.name"
                                label="Name"
                        />
                    </v-flex>
                    <v-flex xs12>
                        <v-text-field
                                v-model.number="account.iban"
                                label="IBAN"
                                mask="AA## #### #### #### #### #"
                        />
                    </v-flex>
                    <v-flex xs12>
                        <v-text-field
                                v-model.number="account.currentBalance"
                                label="Kontostand"
                                type="number"
                        />
                    </v-flex>
                    <v-flex xs12>
                        <v-text-field
                                v-model="account.note"
                                label="Notizen"
                        />
                    </v-flex>
                </v-layout>
            </v-card-text>
            <v-card-actions>
                <v-btn flat @click="cancel">abbrechen</v-btn>
                <v-btn flat @click="save">speichern</v-btn>
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
        showForm: boolean;

        cancel() {
            this.$emit('cancel');
        }

        save() {
            this.$emit('save', this.account)
        }
    }
</script>