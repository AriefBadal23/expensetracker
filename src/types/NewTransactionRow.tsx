import type {Transaction} from "./Transaction.tsx";
import type { Dispatch, SetStateAction } from "react";
import type {Bucket} from "./Bucket.tsx";


export interface NewTransactionRow {
  showModal?: boolean,
  SetShowModal?: (showModal:boolean) => void,
  isUpdateForm: boolean,
  transactionID?: number
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
  setUpdateForm: Dispatch<SetStateAction<boolean>>
  buckets: Bucket[]
}


