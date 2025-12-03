import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateForm from "./components/CreateForm";
import { Buckets } from "./types/Buckets";
import type { Bucket } from "./types/Bucket";
import Bucket from "./components/Bucket";

function App() {
  // Hold the transaction as state
  const [transactions, setTransactions] = useState([
    {
      description: "Monthly Subscription",
      amount: 49.99,
      bucket: Buckets.Shopping,
    },
    {
      description: "Groceries",
      amount: 87.45,
      bucket: Buckets.Groceries,
    },
  ]);

  // Hold  the current bucket values as state
  const [buckets, setBucket] = useState<Bucket[]>([
    {
      id: 1,
      name: "Salary",
      total: 0,
      icon: "💰",
      type: Buckets.Salary,
    },
    {
      id: 2,
      name: "Shopping",
      total: 49.99,
      icon: "🛒",
      type: Buckets.Shopping,
    },
    {
      id: 3,
      name: "Groceries",
      total: 87.45,
      icon: "🏪",
      type: Buckets.Groceries,
    },
  ]);

  function UpdateBucketAmount(id: number, amount: number): void {
    setBucket(
      buckets.map((b) =>
        b.id === id ? { ...b, total: b.total + Number(amount) } : b
      )
    );
  }
  function UpdateTable(
    newamount: number,
    newdescription: string,
    newbucket: Buckets
  ): void {
    setTransactions((prev) => [
      ...prev,
      {
        amount: newamount,
        description: newdescription,
        bucket: newbucket,
      },
    ]);
  }
  return (
    <>
      <BucketList buckets={buckets} />
      <TransactionTable transactions={transactions} />
      <CreateForm
        updateTable={UpdateTable}
        updateBucketAmount={UpdateBucketAmount}
      />
    </>
  );
}

export default App;
