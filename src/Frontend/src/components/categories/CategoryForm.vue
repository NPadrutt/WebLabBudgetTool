<template>
    <v-dialog
            :value="value"
            @input="cancel"
            max-width="600px"
    >
        <v-card>
            <v-card-title>
                <div>
                    <h3 class="headline">Category</h3>
                </div>
            </v-card-title>
            <v-card-text>
                <v-form
                        ref="form"
                        v-if="category"
                        v-model="validFormData"
                >
                    <v-container grid-list-md>
                        <v-layout wrap>
                            <v-flex xs12>
                                <v-text-field
                                        v-model="category.name"
                                        label="Name"
                                        required
                                        :rules="[v => !!v || 'Give the category a name!']"
                                />
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field
                                        v-model="category.note"
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
    import {Category} from "../../types/Category";

    @Component
    export default class CategoryForm extends Vue {
        @Prop()
        category: Category;

        @Prop()
        value: boolean;

        validFormData: boolean = true;

        submit() {
            if ((<HTMLFormElement>this.$refs.form).validate()) {
                this.$emit('save', this.category);
            }
        }

        cancel() {
            (<HTMLFormElement>this.$refs.form).reset();
            this.$emit('input');
        }
    }
</script>