import type { Transaction } from "./Transaction";

export interface NewTransactionRow {
  updateTable: (amount: number, description: string) => Transaction[] | void | undefined;
}
