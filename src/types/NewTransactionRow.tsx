import type { Transaction } from "./Transaction";
import type { Buckets } from "../types/Buckets";

export interface NewTransactionRow {
  updateTable: (
    amount: number,
    description: string,
    bucket: Buckets
  ) => Transaction[];

  updateBucketAmount: (name: Buckets, amount: number) => void;
}
