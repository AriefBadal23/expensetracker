
export enum BucketTypes  {
  Income = "income",
  Expense = "expense"
}

export  interface Bucket {
  bucketTotal: number,
  bucket: {
    id?: number;
    name: string;
    icon: string;
    type?: BucketTypes
  }
}
