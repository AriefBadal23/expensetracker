export interface NewTransactionRow {
  updateTable: (
    amount: number,
    description: string,
    bucketId: number,
    created_at: string,
    isExpense: boolean
  ) => void;

  // updateBucketAmount: (name: Buckets, amount: number) => void;
}
