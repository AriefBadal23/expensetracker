export interface NewTransactionRow {
  updateTable: (
    amount: number,
    description: string,
    bucketId: number,
    created_at: Date,
    isIncome: boolean
  ) => void;

  // updateBucketAmount: (name: Buckets, amount: number) => void;
}
