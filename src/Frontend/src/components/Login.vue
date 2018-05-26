<template>
    <v-app id="inspire" dark>
        <v-content>
            <v-container fluid fill-height>
                <v-layout align-center justify-center>
                    <v-flex xs12 sm8 md4>
                        <v-card class="elevation-12">
                            <v-toolbar dark color="primary">
                                <v-toolbar-title>WEBLAB Budget Tool</v-toolbar-title>
                            </v-toolbar>
                            <v-card-text>
                                <v-form @submit.prevent="login">
                                    <v-text-field v-model="username" prepend-icon="person" name="login"
                                                  label="Login" type="text"/>
                                    <v-text-field v-model="password" prepend-icon="lock" name="password"
                                                  label="Password" type="password"/>
                                    <v-btn style="display: none" type="submit">Login</v-btn>
                                </v-form>
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="primary" @click="login">Login</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-flex>
                </v-layout>
            </v-container>
        </v-content>
    </v-app>
</template>

<script lang="ts">
    import {Vue, Component, Prop} from 'vue-property-decorator';
    import AuthService from "../service/AuthService";

    @Component
    export default class Login extends Vue {
        @Prop()
        redirectTarget: string;

        username: string = '';

        password: string = '';

        async login() {
            let authenticated = await AuthService.login(this.username, this.password);
            if (authenticated) {
                let location = this.redirectTarget !== undefined ? this.redirectTarget : {name: 'Dashboard'};
                this.$router.push(location);
            }
        }
    }

</script>