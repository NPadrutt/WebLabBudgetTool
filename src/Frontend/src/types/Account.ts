export interface Account {
    id: number;
    name: String;
    iban: String;
    currentBalance?: number;
    note: String;
    isOverdrawn: boolean;
    isExcluded: boolean;
}