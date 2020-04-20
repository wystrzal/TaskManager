export interface Task {
  taskId: number;
  name: string;
  status: string;
  priority: string;
  timeToEnd: number;
  taskOwner: number;
  projectId: number;
  projectName: string;
  taskOwnerPhoto: number;
}
