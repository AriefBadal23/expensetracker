import type {Transaction} from "./Transaction.tsx";
import type { Dispatch, SetStateAction } from "react";


export interface NewTransactionRow {
  showModal?: boolean,
  SetShowModal?: (showModal:boolean) => void,
  isUpdateForm: boolean,
  transactionID?: number
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
}


