export interface Task {
  taskId: number;
  name: string;
  status: string;
  priority: string;
  timeToEnd: number;
  taskOwner: number;
  taskOwnerPhoto: number;
  taskOwnerNick: string;
}
