<style>
    .blockquote footer {
        font-size: 80%;
    }
</style>

<template>
    <v-app id="inspire" dark>
        <v-navigation-drawer
                :clipped="$vuetify.breakpoint.lgAndUp"
                v-model="drawer"
                fixed
                app
        >
            <v-list dense class="my-3">
                <v-list-tile :to="{name: 'Accounts'}">
                    <v-list-tile-action>
                        <v-icon>account_balance_wallet</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title class="subheading">Accounts</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
                <v-list-tile :to="{name: 'Categories'}">
                    <v-list-tile-action>
                        <v-icon>folder</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title class="subheading">Categories</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
                <v-list-tile :to="{name: 'Payments'}">
                    <v-list-tile-action>
                        <v-icon>payment</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title class="subheading">Payments</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
            </v-list>

            <v-spacer/>

            <v-container>
                <v-layout>
                    <blockquote v-if="cite.cite" class="blockquote my-5">
                        <p>{{cite.cite}}</p>
                        <footer>
                            ⚽️ {{cite.firstName}} {{cite.lastName}}
                        </footer>
                    </blockquote>
                </v-layout>
            </v-container>
        </v-navigation-drawer>
        <v-toolbar
                :clipped-left="$vuetify.breakpoint.lgAndUp"
                color="primary"
                dark
                fixed
                permanent
                app>
            <v-toolbar-side-icon @click.stop="toggleNavigation"></v-toolbar-side-icon>
            <v-toolbar-title>{{title}}</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-menu bottom left>
                <v-btn slot="activator" icon dark>
                    <v-icon>person</v-icon>
                </v-btn>
                <v-list>
                    <v-list-tile @click="logout">
                        <v-list-tile-title>Logout</v-list-tile-title>
                    </v-list-tile>
                </v-list>
            </v-menu>
        </v-toolbar>
        <v-content>
            <router-view @mounted="mountedChild"/>
        </v-content>
        <v-footer color="primary" app class="px-5">
            <span class="white--text">&copy; 2018 Nino Padrutt und Peter Gisler</span>
        </v-footer>
    </v-app>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from 'vue-property-decorator'
    import AuthService from "../service/AuthService";
    import cites from "../assets/cites"

    @Component
    export default class Dashboard extends Vue {
        @Prop()
        source: String;

        title: string = 'WEBLAB Budget Tool';
        drawer: boolean = false;

        cite?: {[index:string]:string} = {};

        mountedChild(title: string) {
            this.title = title;
        }

        logout() {
            AuthService.logout();
            this.$router.push({name: 'Login'});
        }

        toggleNavigation() {
            this.drawer = !this.drawer;

            if (this.drawer === true) {
                this.cite = this.getRandomCite();
            }
        }

        getRandomCite() {
            let index = Math.floor(Math.random() * (cites.length + 1));
            return cites[index];
        }

    }
</script>