///////Login and Signup form animation to switch

const loginText = document.querySelector(".title-text .login");
const loginForm = document.querySelector("form.login");
const loginBtn = document.querySelector("label.login");
const signupBtn = document.querySelector("label.signup");
const signupLink = document.querySelector("form .signup-link a");
//// An animation to switch the forms sides by moving the element to the left or right
signupBtn.onclick = (()=>{
loginForm.style.marginLeft = "-50%";
loginText.style.marginLeft = "-50%";
});
loginBtn.onclick = (()=>{
loginForm.style.marginLeft = "0%";
loginText.style.marginLeft = "0%";
});
signupLink.onclick = (()=>{
signupBtn.click();
return false;
});




document.getElementById("loginSubmit").onclick = function () {
    var emailInput = document.getElementById("emailInput");
    if (emailInput.value === "admin") {
        emailInput.removeAttribute("required");
        emailInput.setCustomValidity(""); // Remove any validation message
    }
};



////// User login and signup info saving and registering

//// Saves the entered data into localStorage array
//function saveUserData(email, password) {
//  const users = JSON.parse(localStorage.getItem('users')) || [];
//  users.push({ email, password });
//  localStorage.setItem('users', JSON.stringify(users));
//}
//function userExists(email, password) {
//  const users = JSON.parse(localStorage.getItem('users')) || [];
//  return users.some(user => user.email === email && user.password === password);
//}
////// Checks if signup data is valid or new
//function handleSignup() {
//  const email = document.querySelector("form.signup input[type='text']").value;
//  const password = document.querySelector("form.signup input[type='password']").value;
//  if (!userExists(email, password)) {
//    saveUserData(email, password);
//    alert('Account created successfully. You can now log in.');
//  } else {
//    alert('This email is already registered. Please use a different email.');
//  }
//}
////// Checks if login info exists
//function handleLogin() {
//  const email = document.querySelector("form.login input[type='text']").value;
//  const password = document.querySelector("form.login input[type='password']").value;
//  if (userExists(email, password)) {
//    alert('Login successful. Redirecting to the start page.');
//    window.location.href = 'index.html';
//  } else {
//    alert('Invalid email or password. Please try again.');
//  }
//}
//document.querySelector("form.signup").addEventListener('submit', function (event) {
//  event.preventDefault();
//  handleSignup();
//});
//document.querySelector("form.login").addEventListener('submit', function (event) {
//  event.preventDefault();
//  handleLogin();
//});

////// Shop category selector
function toggleCategory(category) {
  const products = document.querySelectorAll('#cat_main .prod');

  //// Uses the shops product cards listed category(.item .category) to sort items by mathig them 
  //// to the category tied to the selected picture(toggleCategory('Laptop')).
  products.forEach(product => {
      const productCategory = product.querySelector('.category').innerText.trim();

      if (category === 'All' || productCategory.toLowerCase() === category.toLowerCase()) {
          //// Show the product if category matches
          product.classList.remove('hidden');
      } else {
          //// Hide the product if category doesn't match
          product.classList.add('hidden');
      }
  });

  //// Redirect to builder.html if Custom Builder category is selected
  if (category.toLowerCase() === 'custom') {
      window.location.href = './builder.html';
  }
  ////To scroll to the shops items when a category is selected
  const catMainElement = document.getElementById("cat_main");
  catMainElement.scrollIntoView({ behavior: "smooth", block: "start" });
}



////// Product page table switcher
function switchTab(tabId) {
  var tabs = document.querySelectorAll('.product_info_button ul li a');
  //// Remove all active and show classes from all tabs
  tabs.forEach(function(t) {
      t.classList.remove('active');
      t.classList.remove('show');
  });
  var targetTab = document.querySelector(`.product_info_button ul li a[href="#${tabId}"]`);
  //// Add active and show classes to the clicked tab
  if (targetTab) {
      targetTab.classList.add('active');
      targetTab.classList.add('show');
  }
  var tabPanes = document.querySelectorAll('.tab-pane');
  //// Remove show and active class from all tab-panes
  tabPanes.forEach(function(pane) {
      pane.classList.remove('show');
      pane.classList.remove('active'); 
  });
  //// Add show and active class to the target tab-pane
  var targetPane = document.getElementById(tabId);
  if (targetPane) {
      targetPane.classList.add('show');
      targetPane.classList.add('active');
  }
}
////Product pages image gallery switcher for main image
function changeImage(newSrc) {
  document.getElementById('mainImage').src = newSrc;
}


////// Support page logic

////For opening the select file dialog in windows
function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader(); ////Reads th files contents
    reader.onload = function(e) { ////When reading of the file is completed -
      //// - starts the event for changing the html sections from ready to uploadto uploaded
      $('.image-upload-wrap').hide();
      $('.file-upload-image').attr('src', e.target.result); ////Shows the small image preview by setting the src for the image from the read file.
      $('.file-upload-content').show();
      $('.image-title').html(input.files[0].name); ////Same as src but for title
    };
    reader.readAsDataURL(input.files[0]);
  } else {
    removeUpload(); ////In case file is not chosen
  }
}
////Removes the added image with a fresh clone of the file-upload-input element in html
function removeUpload() {
  $('.file-upload-input').replaceWith($('.file-upload-input').clone());
  $('.file-upload-content').hide();
  $('.image-upload-wrap').show();
}
////Binding event handlers for the html element for drag and drop
$('.image-upload-wrap').bind('dragover', function () {
		$('.image-upload-wrap').addClass('image-dropping');
	});
	$('.image-upload-wrap').bind('dragleave', function () {
		$('.image-upload-wrap').removeClass('image-dropping');
});

////// Builder page logic

var price_current = 0;
var gpuPrice = 0;
var cpuPrice = 0;
var ssdPrice = 0;
var colPrice = 0;

////Function for picking a laptop part, all are the same except the variable names
function pickCPU(clickedDiv) {
  const cpu_options = document.querySelectorAll('#build_main .lt_items .cpu_pick');
  const cpu_img = document.getElementById('cpu');
  const cpu_img1 = document.getElementById('cpu1');
  let variant = 1; ////For picking one of the variants
  cpu_options.forEach(cpu_option => {
    ////Used to select only one of the laptop part divs and set coresponding values
    if (cpu_option === clickedDiv) {
      cpu_option.classList.add('focused_div');
      variant = 2;
      cpuPrice = 199;
    } else {
      cpu_option.classList.remove('focused_div');
      variant = 3;
      cpuPrice = 219.98;
    }
  });
  ////Depending on variant, cycles the correct image
  if (variant === 2) {
    cpu_img1.classList.remove('hidden');
    cpu_img.classList.add('hidden');
  } else {
    cpu_img.classList.remove('hidden');
    cpu_img1.classList.add('hidden');
  }
  ////Once price is set, gives the value to theprice calculator
  updatePrice();
}
function pickGPU(clickedDiv1) {
  const gpu_options = document.querySelectorAll('#build_main .lt_items .gpu_pick');
  const gpu_img = document.getElementById('gpu');
  const gpu_img1 = document.getElementById('gpu1');
  let variant = 1;
  gpu_options.forEach(gpu_option => {
    if (gpu_option === clickedDiv1) {
      gpu_option.classList.add('focused_div');
      variant = 2;
      gpuPrice = 399.99;
    } else {
      gpu_option.classList.remove('focused_div');
      variant = 3;
      gpuPrice = 459;
    }
  });
  if (variant === 2) {
    gpu_img1.classList.remove('hidden');
    gpu_img.classList.add('hidden');
  } else {
    gpu_img.classList.remove('hidden');
    gpu_img1.classList.add('hidden');
  }
  updatePrice();
}

function pickSSD(clickedDiv2) {
  const ssd_options = document.querySelectorAll('#build_main .lt_items .ssd_pick');
  const ssd_img = document.getElementById('storage');
  const ssd_img1 = document.getElementById('storage1');
  let variant = 1;
  ssd_options.forEach(ssd_option => {
    if (ssd_option === clickedDiv2) {
      ssd_option.classList.add('focused_div');
      variant = 2;
      ssdPrice = 199.99;
    } else {
      ssd_option.classList.remove('focused_div');
      variant = 3;
      ssdPrice = 89.99;
    }
  });
  if (variant === 2) {
    ssd_img1.classList.remove('hidden');
    ssd_img.classList.add('hidden');
  } else {
    ssd_img.classList.remove('hidden');
    ssd_img1.classList.add('hidden');
  }
  updatePrice();
}
function pickCol(clickedDiv3) {
  const col_options = document.querySelectorAll('#build_main .lt_items .col_pick');
  const col_img_top = document.getElementById('frame_top');
  const col_img_bottom = document.getElementById('frame_bottom_black');
  const col_img_bottom2 = document.getElementById('frame_bottom');
  let variant = 1;
  col_options.forEach(col_option => {
    if (col_option === clickedDiv3) {
      col_option.classList.add('focused_div_col');
      col_img_top.classList.remove('hidden');
      variant = 2;
      colPrice = 100;
    } else {
      col_option.classList.remove('focused_div_col');
      variant = 3;
      colPrice = 100;
    }
  });
  if (variant === 2) {
    col_img_bottom2.classList.remove('hidden');
    col_img_bottom.classList.add('hidden');
  } else {
    col_img_bottom.classList.remove('hidden');
    col_img_bottom2.classList.add('hidden');
  }
  updatePrice();
}
function updatePrice() {
  ////All prive variables
  let prices = [cpuPrice, gpuPrice, ssdPrice, colPrice];
  ////Gets all prices that are not yet set for inactive parts. 
  ////They are NaN values if if not picked
  let validPrices = prices.filter(price => !isNaN(price));
  if (validPrices.length > 0) {
    ////Using reduce calculates all relevant prices from the array into one value
    price_current = validPrices.reduce((sum, price) => sum + price, 0);
  } else {
    price_current = 0; 
  }
  ////Update the HTML visual element to new price, by converting to string
  document.getElementById("price1").innerHTML = "$" + price_current.toFixed(2);
}

