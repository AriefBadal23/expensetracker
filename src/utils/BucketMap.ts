import { Buckets } from "../types/Buckets";

export const BucketToId: Record<Buckets, number> = {
  [Buckets.Salary]: 1,
  [Buckets.Groceries]: 2,
  [Buckets.Shopping]: 3,
};

export const IdToBucket: Record<number, Buckets> = {
  1: Buckets.Salary,
  2: Buckets.Groceries,
  3: Buckets.Shopping,
};
