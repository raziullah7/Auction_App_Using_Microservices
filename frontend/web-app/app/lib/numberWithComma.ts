// adds commas in large numbers
export function numberWithCommas(amount: number) {
  return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
