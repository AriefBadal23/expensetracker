import type { Buckets } from "./Buckets";

export  interface Bucket {
  id: number;
  name: string;
  total: number;
  icon: string;
  type: Buckets
}
