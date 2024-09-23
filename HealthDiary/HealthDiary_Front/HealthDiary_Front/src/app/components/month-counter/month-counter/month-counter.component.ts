import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-month-counter',
  templateUrl: './month-counter.component.html',
  styleUrl: './month-counter.component.css'
})
export class MonthCounterComponent implements OnInit {
  public days: number[] = [];
  public currentDay: number = 0;
  public daysRemaining: number = 0;
  public currentMonth: string = '';
  public filledDots: number = 0;

  ngOnInit(): void {
    this.getDays();
    this.startFilling();
  }

  private getDays() {
    const now = new Date();
    const year = now.getFullYear();
    const month = now.getMonth() + 1;
    this.currentDay = now.getDate();
    const daysInMonth = new Date(year, month, 0).getDate();
    this.daysRemaining = daysInMonth - this.currentDay;

    this.currentMonth = now.toLocaleString('en-US', { month: 'long' });

    this.days = Array.from({ length: daysInMonth }, (_, i) => i + 1);
  }

  private startFilling() {
    this.filledDots = this.currentDay; 
  }
}
