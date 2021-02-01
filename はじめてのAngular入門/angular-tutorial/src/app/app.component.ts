import { Component } from '@angular/core';

export class Member {
  id: number;
  name: string;
}

@Component({
  selector: 'my-app',
  template: `
    <h1>{{title}}</h1>
    <h2>{{member.name}}</h2>
    <div><label>id: </label>{{member.id}}</div>
    <div><label>name: </label>{{member.name}}</div>
  `,
})
export class AppComponent  {
  title = '自社社員名簿';
  member: Member = {
    id: 1,
    name: '山田太郎'
  };
}
