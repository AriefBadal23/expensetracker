// import Bucket from "./Bucket";
import "../styles/BucketList.css";
// import type { Buckets } from "../types/Buckets";
import Bucket from "./Bucket";
import type { Bucket as BucketType } from "../types/Bucket";

interface BucketListProps {
  buckets: BucketType[];
}

const BucketList = ({ buckets }: BucketListProps) => {
  return (
    <>
      <h1>Transaction Overview</h1>
      <div className="bucket-list">
        {buckets.map((b: BucketType) => {
          return <Bucket name={b.name} amount={b.total} icon={b.icon} />;
        })}
      </div>
    </>
  );
};

export default BucketList;
// altijd een extra return wanneer je map() gebruikt;
