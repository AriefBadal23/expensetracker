
export interface NewTransactionRow {
  updateTable: (amount: number, description: string, bucketId: number) => void;

  // updateBucketAmount: (name: Buckets, amount: number) => void;
}
