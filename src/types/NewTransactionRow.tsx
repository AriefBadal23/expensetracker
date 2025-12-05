import type { Buckets } from "../types/Buckets";

export interface NewTransactionRow {
  updateTable: (
    amount: number,
    description: string,
    bucket: Buckets
  ) => void;

  updateBucketAmount: (name: Buckets, amount: number) => void;
}
