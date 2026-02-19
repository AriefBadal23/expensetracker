export interface NewTransactionRow {
  updateTable?: (
    amount: number,
    description: string,
    bucketId: number,
    created_at: Date,
    isIncome: boolean
  ) => void,
  showModal?: boolean,
  SetShowModal?: (showModal:boolean) => void,
  isUpdateForm: boolean,
  transactionID?: number
}


