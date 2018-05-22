<template>
    <div id="app">
        <dashboard/>
    </div>
</template>

<script lang="ts">
    import Dashboard from "./components/Dashboard";
    export default {
        name: 'App',
        components: {Dashboard}
    }
</script>

<style>
    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        color: #2c3e50;
    }
</style>