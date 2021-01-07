import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Udemy App';
  users: any;

  constructor(private accountService: AccountService){}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(): void{
    const localStorageUser: string | null = localStorage.getItem('user');
    if (!localStorageUser){
      this.accountService.setCurrentUser(undefined);
      return;
    }
    const user: User = JSON.parse(localStorageUser);
    this.accountService.setCurrentUser(user);
  }
}
