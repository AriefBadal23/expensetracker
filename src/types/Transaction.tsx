import type { Buckets } from "./Buckets";

export interface Transaction {
  amount: number;
  description: string;
  bucket: Buckets;
}
