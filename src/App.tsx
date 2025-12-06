import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import { Buckets } from "./types/Buckets";
import type { Bucket } from "./types/Bucket";
import CreateFormModal from "./components/CreateFormModal";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import BucketDetail from "./components/BucketDetail";

function App() {
  // Hold the transaction as state
  const [transactions, setTransactions] = useState([
    {
      description: "Monthly Subscription",
      amount: 49.99,
      bucket: Buckets.Shopping,
    },
    {
      description: "A lot of Health Groceries",
      amount: 87.45,
      bucket: Buckets.Groceries,
    },
    {
      description: "A lot of Snacks",
      amount: 18.45,
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

  function UpdateBucketAmount(name: Buckets, amount: number): void {
    // TODO: name could be and ID in the future.
    setBucket(
      buckets.map((b) =>
        b.name === name ? { ...b, total: b.total + Number(amount) } : b
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
      <BrowserRouter>
        <Routes>
          <Route
            path="/"
            element={
              <>
                <BucketList buckets={buckets} />
                <TransactionTable transactions={transactions} />
                <CreateFormModal
                  updateBucketAmount={UpdateBucketAmount}
                  updateTable={UpdateTable}
                />
              </>
            }
          />
          <Route
            path=":name"
            element={<BucketDetail transactions={transactions} />}
          />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
