export interface Task {
  taskId: number;
  name: string;
  status: string;
  priority: string;
  timeToEnd: number;
  projectType: string;
  taskOwner: number;
  projectOwner: number;
}
