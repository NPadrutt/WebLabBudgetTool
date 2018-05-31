<style>
    .theme--light .chip {
        color: rgba(0, 0, 0, 0.6);
    }

    .chip {
        text-transform: uppercase;
        font-size: 75%;
    }
</style>

<template>
    <v-data-table
            :headers="headers"
            :items="payments"
            :pagination.sync="pagination"
            class="elevation-1"
    >
        <template slot="items" slot-scope="props">
            <td class="text-xs-left">{{ formatDate(props.item.date) }}</td>
            <td class="text-xs-left">{{ props.item.account.name }}</td>
            <td class="text-xs-left">{{ props.item.category ? props.item.category.name : '-'}}</td>
            <td class="text-xs-left">{{ props.item.note}}</td>
            <td class="text-xs-right">{{ formatCurrency(props.item.amount) }}</td>
            <td class="text-xs-center">
                <i class="material-icons" v-if="props.item.isCleared">check</i>
                <i class="material-icons" v-if="!props.item.isCleared">close</i>
            </td>
            <td class="text-xs-center">
                <v-chip small disabled>{{ formatPaymentType(props.item.type) }}</v-chip>
            </td>
        </template>
    </v-data-table>
</template>

<script lang="ts">
    import {Component, Emit, Inject, Model, Prop, Provide, Vue, Watch} from 'vue-property-decorator'
    import {Payment, PaymentType} from "../../types/Payment";
    import {DateService} from "../../service/DateService";

    @Component
    export default class PaymentList extends Vue {
        @Prop()
        payments: Payment[];

        pagination = {
            sortBy: 'date',
            descending: true
        };

        headers = [
            {text: 'Date', value: 'date'},
            {text: 'Account', value: 'account.name'},
            {text: 'Category', value: 'category.name'},
            {text: 'Notes', value: 'note'},
            {text: 'Amount', value: 'amount', align: 'right'},
            {text: 'Is Cleared', value: 'isCleared', align: 'center'},
            {text: 'Type', value: 'paymentType', align: 'center'}
        ];

        formatCurrency(value: number) {
            let val = (value / 1).toFixed(2);
            return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "'")
        }

        formatDate(date: Date) {
            return DateService.format(date, 'DD.MM.YYYY');
        }

        formatPaymentType(paymentType: PaymentType) {
            let output: string;
            switch (paymentType) {
                case PaymentType.INCOME:
                    output = 'income';
                    break;
                case PaymentType.EXPENSE:
                    output = 'expense';
                    break;
                case PaymentType.TRANSFER_IN:
                    output = 'transfer in';
                    break;
                case PaymentType.TRANSFER_OUT:
                    output = 'transfer out';
                    break;
                default:
                    output = 'unknown';
                    break;
            }
            return output;
        }
    }
</script>