<template>
    <v-menu
            lazy
            :close-on-content-click="false"
            v-model="menu"
            transition="scale-transition"
            offset-y
            full-width
            :nudge-right="40"
            max-width="290px"
            :disabled="disabled"
            min-width="290px">

        <v-text-field
                :disabled="disabled"
                :rules="rules"
                slot="activator"
                :label="label"
                :value="dateFormatted"
                append-icon="event"
                readonly/>

        <v-date-picker
                v-model="dateData"
                :disabled="disabled"
                @input="updateDate(dateData)"
                :min="minDate"
                :max="maxDate"
                :allowed-dates="allowedDates"
                first-day-of-week="1"
                no-title
                scrollable
                actions
                autosave/>
    </v-menu>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import {DateService} from "../service/DateService";

    @Component
    export default class DatePicker extends Vue {
        @Model('update')
        date: string;

        @Prop()
        label: string;

        @Prop()
        rules: any[];

        @Prop({default: false})
        disabled: boolean;

        @Prop()
        minDate: string;

        @Prop()
        maxDate: string;

        @Prop()
        allowedDates: Function;

        @Watch('date')
        dateDataMomentWatch(dateData: string) {
            this.date = dateData;
            this.dateData = this.parseDate(this.date);
            this.dateFormatted = this.formatDate(this.date);
        }

        dateData: string | null = null;
        dateFormatted: string | null = null;
        menu: boolean = false;

        mounted() {
            if (this.date !== null) {
                this.dateData = this.parseDate(this.date);
                this.dateFormatted = this.formatDate(this.date);
            }
        }

        formatDate(date: string): string | null {
            if (!date) {
                return null
            }

            return DateService.format(date, 'DD.MM.YYYY');
        }

        parseDate(date: string): string | null {
            if (!date) {
                return null
            }

            return DateService.format(date, 'YYYY-MM-DD');
        }

        updateDate(date: string) {
            this.dateFormatted = this.formatDate(date);

            let tempDate;

            if (this.date) {
                tempDate = new Date(this.date);

            } else {
                tempDate = new Date(date);

            }

            tempDate.setDate(+date.split('-')[2]);
            tempDate.setMonth((+date.split('-')[1]) - 1);
            tempDate.setFullYear(+date.split('-')[0]);

            this.menu = !this.menu;
            this.$emit('update', DateService.toISOString(tempDate));
        }
    }
</script>