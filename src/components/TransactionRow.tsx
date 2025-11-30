interface TransactionRowProps {
  description: string;
  amount: number;
}

const TransactionRow = ({ description, amount }: TransactionRowProps) => {
  return (
    <tr>
      <td>{description}</td>
      <td>€{amount}</td>
    </tr>
  );
};

export default TransactionRow;
