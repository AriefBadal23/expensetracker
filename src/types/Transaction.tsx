export type Transaction = {
  id?: number;
  bucketId: number;
  description: string;
  amount: number;
  createdAt: Date;
};
