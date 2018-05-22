<template>
    <v-dialog v-if="showForm" v-model="showForm">
        <v-card>
            <v-card-title>
                <div>
                    <h3 class="headline">Category</h3>
                </div>
            </v-card-title>
            <v-card-text>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-text-field
                                v-model="category.name"
                                label="Name"
                        />
                    </v-flex>
                    <v-flex xs12>
                        <v-text-field
                                v-model="category.note"
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
    import {Category} from "../../types/Category";

    @Component
    export default class CategoryForm extends Vue {
        @Prop()
        category: Category;

        @Prop()
        showForm: boolean;

        cancel() {
            this.$emit('cancel');
        }

        save() {
            this.$emit('save', this.category)
        }
    }
</script>