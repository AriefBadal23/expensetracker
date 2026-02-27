export type Transaction = {
  id?: number;
  bucketId: number;
  userId?: number; // later this will be non-nullable TODO
  description: string;
  amount: number;
  created_at: Date;
};
