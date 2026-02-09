import type { Transaction } from "./Transaction"


export type BucketTransactionsGroup = {
    month: number,
    transactions: Transaction[]
}
export type ChartBucketTransaction = {
    labels: string[],
    totals: number[],
    month: number,
    year: string,
    bucket: number
}