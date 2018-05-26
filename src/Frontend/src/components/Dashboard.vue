<template>
    <v-app id="inspire">
        <v-navigation-drawer
                :clipped="$vuetify.breakpoint.lgAndUp"
                v-model="drawer"
                fixed
                app
        >
            <v-list dense>
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
        </v-navigation-drawer>
        <v-toolbar
                :clipped-left="$vuetify.breakpoint.lgAndUp"
                color="indigo"
                dark
                fixed
                permanent
                app>
            <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
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
            <router-view/>
        </v-content>
        <v-footer color="indigo" app>
            <span class="white--text">&copy; 2017</span>
        </v-footer>
    </v-app>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from 'vue-property-decorator'
    import AuthService from "../service/AuthService";

    @Component
    export default class Dashboard extends Vue {
        @Prop()
        source: String;

        title: string = 'WEBLAB Budget Tool';
        drawer: boolean = false;

        logout() {
            AuthService.logout();
            this.$router.push({name: 'Login'});
        }

    }
</script>