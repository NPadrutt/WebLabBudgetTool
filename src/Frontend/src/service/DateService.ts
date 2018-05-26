import {format, addDays, subDays, isAfter, getDay, parse} from 'date-fns';

export class DateService {

    public static format(date: Date | string | number, targetFormat?: string, options?: object): string {
        return format(date, targetFormat, options);
    }

    public static addDays(date: Date | string | number, amount: number): string {
        return DateService.toISOString(addDays(date, amount));
    }

    public static subDays(date: Date | string | number, amount: number): string {
        return DateService.toISOString(subDays(date, amount));
    }

    public static isAfter(date: Date | string | number, dateToCompare: Date | string | number): boolean {
        return isAfter(date, dateToCompare);
    }

    public static getDay(date: Date | string | number): number {
        return getDay(date);
    }

    public static toISOString(date: Date | string | number): string {
        return parse(date).toISOString();
    }

    public static sortDateStrings(dates: string[]) {

        let datesToSort: string[] = [];

        for (let date of dates) {
            datesToSort.push(this.format(date, 'YYYY-MM-DD'));
        }

        datesToSort.sort((a, b) => a > b ? 1 : -1);

        return datesToSort;
    }

}