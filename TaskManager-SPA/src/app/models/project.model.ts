export interface Project {
  projectId: number;
  name: string;
  type: string;
  owner: number;
  anyUsers: boolean;
  projectUsersNick: string[];
}
