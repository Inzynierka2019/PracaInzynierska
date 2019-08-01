import { Component, HostListener } from '@angular/core';
import * as $ from 'jquery';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  styles: [`.router-link-active { background-color: red; }`]
})
export class AppComponent {
  title = 'Visualization';

  @HostListener('window:scroll')
  onWindowScroll() {
    this.animateNavBar();
    this.animateScrollTop();
  }

  onPageUp() {
    $('html').animate({ scrollTop: 0 }, {
      duration: 400,
      queue: false,
    }, "swing");
  }

  animateNavBar() {

    console.log("working");

    const navbar = $('nav');
    const hasShowOffClass = navbar.hasClass('scroll-effect');
    const windowOffset = window.pageYOffset;
    const targetScaleInClass = windowOffset > 1 ? true : false;

    if (!hasShowOffClass && targetScaleInClass) {
      navbar.addClass('scroll-effect');
      console.log("add");
    }

    if (hasShowOffClass && !targetScaleInClass) {
      navbar.removeClass('scroll-effect');
      console.log("remove");
    }
  }

  animateScrollTop() {
    const scrollTop = $('.scroll-btn');
    const windowOffset = window.pageYOffset;
    const targetScaleInClass = windowOffset > 100 ? true : false;

    if (!targetScaleInClass) {
      scrollTop.addClass('scale-in');
    }

    if (targetScaleInClass) {
      scrollTop.removeClass('scale-in');
    }
  }
}