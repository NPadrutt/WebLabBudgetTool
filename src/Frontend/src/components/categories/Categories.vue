<template>
    <v-container>
        <v-layout row wrap>
            <v-flex xs12 sm6 pa-1 v-for="category of categories" :key="category.id">
                <v-card>
                    <v-card-title primary-title>
                        <h3 class="headline mb-0">{{category.name}}</h3>
                    </v-card-title>
                    <v-card-text>
                        <p>{{category.note}}</p>
                    </v-card-text>
                </v-card>
            </v-flex>
        </v-layout>
        <v-layout row wrap>
            <v-flex>
                <v-btn @click="newCategory">
                    New Category
                </v-btn>
                <category-form
                        :showForm="showForm"
                        :category="categoryForForm"
                        @cancel="cancelEdit"
                        @save="saveCategory"
                />
            </v-flex>
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
        categories: Category[] = [
            <Category>{
                id: 1,
                name: 'Kategorie A',
                note: 'Alles, das mit A anfängt...'
            },
            <Category>{
                id: 2,
                name: 'Kategorie B',
                note: 'Alles, das mit B anfängt...'
            },
            <Category>{
                id: 3,
                name: 'Kategorie C',
                note: 'Alles, das mit C anfängt...'
            }
        ];

        showForm: boolean = false;
        categoryForForm: Category | null = null;
        apiService: ApiService = new ApiService();

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
            // TODO: implement save
            let response = this.apiService.makeRequest('categories', category, 'POST');
            this.showForm = false;
        }
    }
</script>