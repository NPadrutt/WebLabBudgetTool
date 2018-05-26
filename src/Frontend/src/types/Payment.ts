import {Account} from "./Account";
import {Category} from "./Category";

export interface PaymentWithReferences {
    id: number;
    accountId: number;
    categoryId: number | null;
    date: string;
    amount?: number;
    isCleared: boolean;
    type: PaymentType;
    note: string;
}

export interface Payment extends PaymentWithReferences {
    account: Account;
    category?: Category;
}

export enum PaymentType {
    EXPENSE,
    INCOME,
    TRANSFER,
    TRANSFER_IN,
    TRANSFER_OUT,
}