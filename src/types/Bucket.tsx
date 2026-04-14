import type { Buckets } from "./Buckets";

export  interface Bucket {
  bucketTotal: number,
  bucket: {
    id: number;
    name: string;
    total: number;
    icon: string;
    type: Buckets
  }
}
