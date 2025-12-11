import { useParams } from "react-router-dom";
import { IdToBucket } from "../utils/BucketMap";
import type { Transaction } from "../types/Transaction";
import { useEffect, useState } from "react";
import { Buckets } from "../types/Buckets";

function BucketDetail() {
  const params = useParams();
  const [buckettransactions, setTransactions] = useState<Transaction[]>([]);
  useEffect(() => {
    const fetchTransactions = async () => {
      try {
        const response = await fetch(
          `http://localhost:5286/api/v1/buckets/${params.name}`
        );

        const transactions = await response.json();
        setTransactions(transactions);
      } catch {
        console.log("Failed to fetch data from api");
      }
    };
    fetchTransactions();
  }, [params.name]);

  const bucketName = params.name as Buckets;
  const bucketTransactions = buckettransactions.find(
    (x) => IdToBucket[x.bucketId] === bucketName
  );

  return (
    <>
      <h1>
        {bucketTransactions != null
          ? `${IdToBucket[bucketTransactions?.bucketId]}`
          : `No Transactions for ${params.name}`}
      </h1>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Bucket</th>
            <th scope="col">Description</th>
            <th scope="col">Amount</th>
            <th scope="col">Date</th>
          </tr>
        </thead>
        <tbody>
          {buckettransactions.map((b) => {
            return (
              <tr key={b.id}>
                <td>{IdToBucket[b.bucketId]}</td>
                <td>{b.description}</td>
                <td>€ {b.amount}</td>
                <td>{new Date(b.created_at).toLocaleDateString()}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </>
  );
}

export default BucketDetail;
