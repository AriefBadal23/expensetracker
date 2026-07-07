import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import type { Transaction } from "./types/Transaction";
import Pagination from "./components/Pagination";
import Filter from "./components/Filter";
import Overview from "./components/Overview"
import LoginForm from "./components/LoginForm.tsx";
import Navbar from "./components/NavBar.tsx";
import type {Bucket} from "./types/Bucket.tsx";

function App() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  // TODO dont use Error | undefined
  const [errorMessage, setErrorMessage] = useState<Error | undefined>();
  // buckets state
  const [buckets, setBuckets] = useState<Bucket[]>([]);
  
  return (
      <BrowserRouter>
        <Routes>
          <Route path="/"
                 element={<LoginForm/>}
          />
          <Route
              path="/overview"
              element={<Overview/>}
          />
            
          <Route
              path="/dashboard"
              element={
                <>
                  <Navbar/>
                  <BucketList transactions={transactions} setBuckets={setBuckets} buckets={buckets}/>
                  <Filter/>

                  {/*  Pass here the buckets state*/}
                    
                  <TransactionTable transactions={transactions} setTransactions={setTransactions}
                                    ErrorMessage={errorMessage} buckets={buckets} setBuckets={setBuckets}/>
                  <Pagination setTransactions={setTransactions} setErrorMessage={setErrorMessage}/>
                </>
              }
          />
        </Routes>
      </BrowserRouter>
  );
}

export default App;
