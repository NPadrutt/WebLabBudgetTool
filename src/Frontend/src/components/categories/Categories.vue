<template>
    <v-container class="pa-0">
        <v-layout>
            <v-flex xs12>
                <v-container grid-list-lg>
                    <v-layout row wrap>
                        <v-flex xs12 md6 v-for="category of categories" :key="category.id" flexbox>
                            <v-card height="100%">
                                <v-card-title primary-title>
                                    <h3 class="headline mb-0">{{category.name}}</h3>
                                </v-card-title>
                                <v-card-text>
                                    <p>{{category.note}}</p>
                                </v-card-text>
                            </v-card>
                        </v-flex>
                    </v-layout>
                </v-container>
            </v-flex>
        </v-layout>
        <v-layout justify-center>
            <v-btn color="primary" @click="newCategory">
                New Category
            </v-btn>
            <category-form
                    v-model="showForm"
                    :category="categoryForForm"
                    @save="saveCategory"
            />
        </v-layout>
    </v-container>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import {Category} from "../../types/Category";
    import CategoryForm from "./CategoryForm";
    import ApiService from "../../service/ApiService";

    @Component({
        components: {
            CategoryForm
        }
    })
    export default class Categories extends Vue {
        categories: Category[] = [];
        showForm: boolean = false;
        categoryForForm: Category | null = null;

        mounted() {
            this.$emit('mounted', 'Categories');
            this.loadCategories();
        }

        loadCategories() {
            ApiService.makeRequest('categories')
                .then(response => response.json())
                .then(json => {
                    this.categories = json
                });
        }

        newCategory() {
            let category = <Category>{
                id: 0,
                name: '',
                note: ''
            };
            this.editCategory(category);
        }

        editCategory(category: Category) {
            this.categoryForForm = category;
            this.showForm = true;
        }

        cancelEdit() {
            this.categoryForForm = null;
            this.showForm = false;
        }

        saveCategory(category: Category) {
            ApiService.makeRequest('categories', category, 'POST')
                .then(response => {
                    this.loadCategories();
                    this.showForm = false;
                });
        }
    }
</script>