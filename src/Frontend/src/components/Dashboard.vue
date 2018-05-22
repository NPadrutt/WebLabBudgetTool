<template>
    <v-app id="inspire">
        <v-navigation-drawer
                v-model="drawer"
                fixed
                app
        >
            <v-list dense>
                <v-list-tile @click="$router.push('/accounts')">
                    <v-list-tile-action>
                        <v-icon>account_balance_wallet</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title>Accounts</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
                <v-list-tile @click="$router.push('/categories')">
                    <v-list-tile-action>
                        <v-icon>folder</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title>Categories</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
                <v-list-tile @click="$router.push('/payments')">
                    <v-list-tile-action>
                        <v-icon>payment</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>
                        <v-list-tile-title>Payments</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
            </v-list>
        </v-navigation-drawer>
        <v-toolbar color="indigo" dark fixed app>
            <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon>
            <v-toolbar-title>Application</v-toolbar-title>
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
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'

    @Component
    export default class Dashboard extends Vue {
        @Prop()
        source: String;

        drawer: boolean = false;
    }
</script>