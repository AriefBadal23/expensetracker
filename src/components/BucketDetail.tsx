import { useParams } from "react-router-dom";
import type { Transaction } from "../types/Transaction";

interface BucketDetailProps {
  transactions: Transaction[];
}
function BucketDetail({ transactions }: BucketDetailProps) {
  const params = useParams();
  const bucket = transactions.filter((t) => t.bucket === params.name);
  const total = new Array<number>();

  return (
    <>
      <h1>{params.name}</h1>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Bucket</th>
            <th scope="col">Description</th>
            <th scope="col">Amount</th>
          </tr>
        </thead>
        <tbody>
          {bucket.map((b) => {
            total.push(b.amount);
            return (
              <>
                <tr>
                  <td>{b.bucket}</td>
                  <td>{b.description}</td>
                  <td>{b.amount}</td>
                </tr>
              </>
            );
          })}
        </tbody>
      </table>
      <h1>€{total.reduce((x, y) => x + y)}</h1>
    </>
  );
}

export default BucketDetail;
