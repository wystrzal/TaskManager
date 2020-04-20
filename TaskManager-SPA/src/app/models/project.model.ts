export interface Project {
  projectId: number;
  name: string;
  type: string;
  owner: number;
  anyUsers: boolean;
  projectUsersId: number[];
  projectUsersNick: string[];
}
